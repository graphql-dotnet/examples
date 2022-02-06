# ASP.NET Core GraphQL Example

```
> ./run.sh
> browse to http://localhost:3000/ui/playground
```

If you like to try GraphQL compiled using [NativeAOT](https://github.com/dotnet/runtimelab/issues/248) which should [happen](https://github.com/dotnet/runtime/issues/61231) in .NET 7

Native AOT (ahead-of-time) compilation in .NET allows startup time in tens of milliseconds, with high performance and predictability, eliminating JIT (just-in-time) compilation. If you are new to AOT compilation, you can start reading about rationale for it [here](https://github.com/dotnet/designs/blob/main/accepted/2020/form-factors.md#native-aot-form-factors).

**Note**. In your applications, due to reflection, you may need to edit RD.xml files provided in this sample. More about it in [the docs](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/rd-xml-format.md)

```
> ./nativeaot.sh
> browse to http://localhost:3000/ui/playground
```
