namespace Dr.Peril.Script.Internals.Abstractions;

internal readonly record struct Source(
    string Path,
    int LineNumber);
