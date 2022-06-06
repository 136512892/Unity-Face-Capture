using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Unity.LiveCapture.CompanionApp.Editor")]
[assembly: InternalsVisibleTo("Unity.LiveCapture.ARKitFaceCapture.Editor")]
[assembly: InternalsVisibleTo("Unity.LiveCapture.VirtualCamera.Editor")]
[assembly: InternalsVisibleTo("Unity.LiveCapture.Tentacle.Editor")]
[assembly: InternalsVisibleTo("Unity.LiveCapture.Ltc.Editor")]

// Internal access needed for testing
[assembly: InternalsVisibleTo("Unity.LiveCapture.Tests.Editor")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
