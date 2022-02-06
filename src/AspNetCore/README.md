# ASP.NET Core GraphQL Example

```
> ./run.sh
> browse to http://localhost:3000/ui/playground
```

If you like to try GraphQL compile using [NativeAOT](https://github.com/dotnet/runtimelab/issues/248) which should [happens](https://github.com/dotnet/runtime/issues/61231) in .NET 7

Native AOT form-factor of .NET allow startup time in tens of milliseconds, and high performance and predictability (no JIT). If you know to AOT, you can start reading about rationale for it [here](https://github.com/dotnet/designs/blob/main/accepted/2020/form-factors.md#native-aot-form-factors).

**Note**. In your applications, due to reflection, you may need to edit RD.xml files provided in this sample. More about it in [the docs](https://github.com/dotnet/runtimelab/blob/feature/NativeAOT/docs/using-nativeaot/rd-xml-format.md)

```
> ./nativeaot.sh
> browse to http://localhost:3000/ui/playground
```
