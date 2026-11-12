#region Purpose
// Source-generated System.Text.Json context for deserializing the embedded theme and SVG-template JSON resources.
#endregion

#region Design
// Source generation (rather than reflection) keeps JSON loading trim- and AOT-friendly; the context is internal because it is only used by ThemeService and PartTemplates.
#endregion

namespace TimeWarp.Multiavatar;

using System.Text.Json.Serialization;

[JsonSerializable(typeof(Theme))]
[JsonSerializable(typeof(Dictionary<string, string>))]
[JsonSerializable(typeof(Dictionary<string, string[]>))]
[JsonSourceGenerationOptions(WriteIndented = false)]
internal sealed partial class MultiavatarJsonContext : JsonSerializerContext
{
}