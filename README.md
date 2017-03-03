# SSharpCrestronExtensionsLibrary
Initial post of SSharpCrestronExtensionsLibrary

This library contains many extensions to both CF and S#.

```
System.Action<T1, T2, T3, T4, ..., T16>
System.Func<T1, T2, T3, T4, ..., T16, TResult>
```

### Enum extensions (missing from CF)
```
HasFlag (from.NET 4.0)
```
### Enum2 static extensions (* missing from CF) [can use Enum = OpenNETCF.Enum2]
```
  GetName *
  GetNames *
  GetUnderlyingType
  GetValues *
  IsDefined
  Parse (* one overload missing)
  ToObject
  Format *
```
  
### Char extensions (missing from CF or .NET 4.0)
```
  ToUpperInvariant
  ToLowerInvariant
  IsDigit (missing overload)
```
  
### CharEx static extensions (missing from CF or .NET 4.0)
```
  ToUpperInvariant
  ConvertFromUtf32
  ConvertToUtf32
  IsHighSurrogate
  IsLowSurrogate
```
  
### ArrayEx static extensions (missing from CF or add in .NET 4.0)
```
  Empty<T>
  TrueForAll<T>
  ForEach<T>
  ConvertAll<TInput, TOutput>
  FindLastIndex<T>
  FindIndex<T>
```

### String extensions (missing from CF or in added in .NET 4.0)
```
  Split (all missing overloads)
  ToLowerInvariant
  ToUpperInvariant
  Remove (missing overload)
```

### StringEx static extensions (missing from CF or added in .NET 4.0)
```
  IsNullOrWhiteSpace
  Join<T>
  Join (all missing overloads)
  Concat<T>
  Concat (all missing overloads)
```
  
### Encoding extensions
```
  GetString (all missing overloads)
```

### .NET 4.0 Classes
```
  WeakReference<T>
  IObserver<T>
  IObervable<T>
```

### Crestron.SimplSharp.CEvent extensions
```
  WaitOne
```
```
Crestron.SimplSharp.AutoResetEvent
Crestron.SimplShatp.ManualResetEvent
```

### Crestron.SimplSharp.CrestronIO.Stream extensions (added in .NET 4.0)
```
  CopyTo
```

### Crestron.SimplSharp.CTimer extensions (missing from S# but in CF)
```
  Change
```

### DnsEx static class (supporting the hosts file)
```
  GetHostAddresses
  GetHostEntry
```
  


