Pinpoint the View
==================

Live example [here](6_Pinpoint_the_origin.live.html).

It is virtually impossible to navigate in 3D view without a rotation. And every rotation has to have its origin.
That is the point which actually doesn't rotate but everything moves around it. By default we snap this origin to the
point of user interaction. But in some scenarios it is useful to fix this point. For example, if you want to show a
single product and want to allow your user to see it from all sides. You can see what I mean [here](6_Pinpoint_the_origin.live.html).

The code is actually very simple. In this case we know that the table has `id = 72002` but you can obviously get the ID
from elsewhere. Retrieving the product and model ids from user interaction is covered in other tutorials.

```js
viewer.zoomTo(72002, 1);
viewer.navigationMode = 'locked-orbit'
viewer.setLockedOrbitOrigin(72002, 1);
```