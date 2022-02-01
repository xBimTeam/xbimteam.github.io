Informational Overlay
====================

Live example [here](7_HTML_overlay.live.html).

It is a common requirement to show additional data on top of the 3D view. While it may seem straightforward, you
actually need get the placement of the object in real world first and then revert the complete chain of transformations
to get the 2D placement on the screen. Also, WebGL uses different conventions for the coordinate system axes directions.
We have done all this work for you, so you can just listen to user interaction events and use them to get values directly
applicable for HTML placement.

Lets say we have some HTML we want to place on top of a product in the 3D view. It may contain semantic data you
brought from xbim powered web server or any other data source.

```html
<div id="float">
    This can contain any arbitrary data you have or get for the element
</div>
```

And you want to keep your HTML on top of the element at all the times, so you will stay in sync with 
browser screen frames:

```js
window.requestAnimationFrame(() => showFloat());
```

The main function you can use to get the coordinates is `getHTMLPositionOfProductCentroid()`. 
Here is the core code which will keep your HTML on top of the 3D element

```js
let showFloat = () => {
    let div = document.getElementById('float');
    let position = viewer.getHTMLPositionOfProductCentroid(36339, 1);
    div.style.left = (position[0] - div.clientWidth / 2) + 'px';
    div.style.top = (position[1] - div.clientHeight / 2) + 'px';
    window.requestAnimationFrame(() => showFloat());
}
viewer.on('loaded', () => {
    // ...
    window.requestAnimationFrame(() => showFloat());
})
```