Informational Overlay
====================

Live example [here](7_HTML_overlay.live.html).

It is a common requirement to show additional data on top of the 3D view. While it may seem straightforward, you
actually need get the placement of the object in real world first and then revert the complete chain of transformations
to get the 2D placement on the screen. Also, WebGL uses different conventions for the coordinate system axes directions.
We have done all this work for you, so you can just listen to user interaction events and use them to get values directly
applicable for HTML placement.

Lets say we have some HTML we want to place on top of a product in the 3D view. It may contain semantic date you
brought from xbim powered web server or any other data source.

```html
<div id="float">
    This can contain any arbitrary data you have or get for the element
</div>
```

And you listen to the user interaction events to get information about the objects:

```js
viewer.on('pick', args => { }); 
viewer.on('mousemove', args => { });
```

The main function you can use to get the coordinates is `getHTMLPositionOfProductCentroid()`. 
Here is the core code which is also highlighting the object to build a visual clue between the 
information and the object in question.

```js
let showFloat = (args) => {
let div = document.getElementById('float');
viewer.resetStyles();
    
    if (args.id == null) {
        div.style.display = 'none';
        return;
    }
    viewer.setStyle(0, [args.id]);
    let position = viewer.getHTMLPositionOfProductCentroid(args.id, args.model);
    div.style.display = 'block';
    div.style.left = (position[0] - div.clientWidth / 2) + 'px';
    div.style.top = (position[1] - div.clientHeight / 2) + 'px';
}
viewer.defineStyle(0, [0,255,0,255]);
viewer.on('pick', showFloat); 
viewer.on('mousemove', showFloat);
```