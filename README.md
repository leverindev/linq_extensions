# LinqExtensions

The **LinqExtensions** is a set of extensions that brings a simple and convenient way to write LEFT / RIGHT / FULL OUTER JOIN queries.

### How to use:

#### LEFT JOIN

```
var query =
    from left in leftEntities.ToOuter()
    join right in rightEntities
        on left.Key equals right.Id
    select new { left, right };
```

that is the equivalent to

```
var query =
    from left in leftEntities
    join right in rightEntities
        on left.Key equals right.Key into joined
    from item in joined.DefaultIfEmpty()
    select new { left, right = item };
```

#### RIGHT JOIN

```
var query =
    from left in leftEntities
    join right in rightEntities.ToOuter()
        on left.Key equals right.Id
    select new { left, right };
```

#### FULL JOIN

```
var query =
    from left in leftEntities.ToOuter()
    join right in rightEntities.ToOuter()
        on left.Key equals right.Id
    select new { left, right };
```

### NuGet

[This package is available on the nuget.org](https://www.nuget.org/packages/LinqExtensions_leverindev)
