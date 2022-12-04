using Kubmill.Models.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Language;
using System.Threading.Tasks;

namespace Kubmill.Repositories
{
    /// <inheritdoc/>
    public class ScriptRepository : IScriptRepository
    {
        private readonly List<ScriptFile> _scripts = new();

        /// <inheritdoc/>
        public async Task LoadScripts()
        {
            var files = Directory.GetFiles("./scripts", "*.ps1", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var script = new ScriptFile(file);
                
                _scripts.Add(script);

                await GetScriptInfo(script);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<ScriptFile> GetAllScripts()
        {
            return _scripts;
        }

        /// <inheritdoc/>
        public IEnumerable<ScriptFile> GetScripts(ScriptContextType type)
        {
            return _scripts.Where(s => s.ContextType == type);
        }

        /// <inheritdoc/>
        public ScriptFile? GetScript(string fileName)
        {
            return _scripts.FirstOrDefault(s => s.FileName == fileName);
        }

        private static async Task GetScriptInfo(ScriptFile script)
        {
            try
            {
                script.Content = await File.ReadAllTextAsync(script.FileName);

                GetFileMeta(script);
            }
            catch (Exception ex)
            {
                script.Errors.Add(ex.Message);
            }
        }

        private static void GetFileMeta(ScriptFile script)
        {
            if (string.IsNullOrEmpty(script.Content)) return;

            GetMetaDescription(script);
            GetParameters(script);            
        }

        private static void GetMetaDescription(ScriptFile script)
        {
            bool inCommentBlock = false;

            var lines = script.Content!.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                var isComment = trimmed.StartsWith("#");

                if (inCommentBlock)
                {
                    inCommentBlock = !trimmed.Contains("#>");
                    
                    if (!inCommentBlock) break;
                }
                else if (trimmed.StartsWith("<#"))
                {
                    inCommentBlock = true;
                }

                if (inCommentBlock || isComment)
                {
                    _ =
                    GetMetaValue(nameof(script.Description), trimmed, v => script.Description = v)
                    ||
                    GetMetaValue(nameof(script.Name), trimmed, v => script.Name = v)
                    ||
                    GetMetaValue(nameof(script.ContextType), trimmed, v => script.ContextType = Enum.Parse<ScriptContextType>(v));
                }
            }
        }

        private static bool GetMetaValue(string key, string line, Action<string> setValue)
        {
            var keyIdx = line.IndexOf($".{key}");

            if (keyIdx >= 0)
            {
                setValue(line[(keyIdx + key.Length + 1)..].Trim());
                return true;
            }

            return false;
        }

        private static void GetParameters(ScriptFile script)
        {
            var ast = Parser.ParseInput(script.Content, out Token[] tokens, out ParseError[] errors);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    script.Errors.Add(error.Message);
                }
            }
            else
            {
                script.Parameters = ast.ParamBlock?.Parameters
                    .Select(pb => new ScriptParameter(pb.Name.ToString(), pb.StaticType, pb.IsMandatory()))
                    ?? new List<ScriptParameter>();
            }            
        }
    }
}
