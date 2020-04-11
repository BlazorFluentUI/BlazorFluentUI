# Contributing

Any and all help is appreciated!  But here are some guidelines:

If you want to add a new control, follow the format of all of the other controls. 
1.  Namespace is `BlazorFabric`.
2.  Control should use `BFUComponentBase` as the base class.
    1.   Add `Styles` and `ClassName` contents to the appropriate element.
3.  Class names mimic the original fabric-react class names with some exceptions:
    1. where style depends on state, use a double dash followed by the state name to create your style. 
       i.e. `ms-Spinner--right` is a class that is placed into the set of class names when `Spinner`'s `LabelPosition` is set to `Right`
4.  New javascript that is potentially usable by all controls should be placed into the `BaseComponent` control's `baseComponent.ts`. 


This is an evolving library mostly put together through trial and error.  There is a lot of disorganization and likely inefficiencies.  I welcome all suggestions.
