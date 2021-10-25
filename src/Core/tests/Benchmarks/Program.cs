using BenchmarkDotNet.Running;

var switcher = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly);

// Prompt which one to run
switcher.Run(args);

// Or uncomment to run all
// switcher.RunAllJoined();