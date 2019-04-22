## Kingdom.Collections.Variants

This module approximates the *C++* [``std::variant``](https://en.cppreference.com/w/cpp/utility/variant), to a point.

If your application requires homogeneous variants, you may simply refer to the strongly typed ``IVariant<T>`` or ``Variant<T>``. However, if your application requires heterogeneous variants, then you may refer to ``IVariant`` or ``Variant``.

*Variants* are similar in that they support a weakly typed ``Value`` in terms of the [``object Variant { get; set; }``](/mwpowellhtx/Kingdom.Collections/src/Kingdom.Collections.Variants/Interfaces/IVariant.cs#L20) property.

You must provide *C#* *Variants* with an instance of [``IVariantConfigurationCollection``](/mwpowellhtx/Kingdom.Collections/src/Kingdom.Collections.Variants/Interfaces/IVariantConfigurationCollection.cs), which informs which types are supported, as well as a couple of supporting cast members backing the [``IEquatable<T>``](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1) and [``IComparable<T>``](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable) interfaces. Such a configuration is a loose approximation to the *C++* ``template <class... Types> class variant`` template parameter ``Types`` list.

It is an imperfect analogy, of course, due to differences between that that the *C# dotnet* runtime nature of generic types.  The key contrast is that instead of scaling horizontally in the runtime type information itself, *Variants* scales vertically, in particular depending on the sort of configuration your application requires.

```C#
// This configuration supports Variants using Integer and Boolean strong types.
// i.e. IVariant<int> x = Variant.Create(1), or IVariant<bool> y = Variant.Create(true)
var config = VariantConfigurationCollection.Create(
    // Lambda argument type names specified for clarity.
    VariantConfiguration.Configure<int>(
        (object x, object y) => (int) x == (int) y
        , (object x, object y) => ((int) x).CompareTo((int) y))
    , VariantConfiguration.Configure<bool>(
        (object x, object y) => (bool) x == (bool) y
        , (object x, object y) => ((bool) x).CompareTo((bool) y))
);
```

These may be furnished statically, factory created, etc, as your runtime requires.