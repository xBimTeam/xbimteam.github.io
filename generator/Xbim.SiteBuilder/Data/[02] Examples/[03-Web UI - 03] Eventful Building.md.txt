Eventful Building
=================

Live example [here](3_Eventful_building.live.html).


In this tutorial we'll have a look on events fired by the viewer on different occasions. 
You can see full live example [here](3_Eventful_building.live.html) 
if you are running this tutorial from web server. Also, make sure your webserver is set up to serve wexBIM files as
a static content.

I'll use ugly obtrusive code with javascript functions defined in *onclick* attributes of HTML elements. This is not the right practise
but I've done it this way for the sake of clarity and simplicity. You are encouraged to follow 
[these guidlines](http://www.w3.org/wiki/The_principles_of_unobtrusive_JavaScript) to write sustainable
and clear web applications.


And now, let's dig into the code. It is pretty easy to register your handler with the following function:

```js	
viewer.on('event_name', callback); //see documentation
```
	
We will show some of the most useful events you might want to watch in the following example. 
It is based on the [previous tutorial](safe-hello-building.html) so we won't list full
page code but rather code snippets. To see a complete example have a look on the code of 
[live example](3_Eventful_building.live.html).

I'll show you complete listing of the new code now. It is pretty much self explaining but I'll cover 
every event in the following text.

```js
viewer.on('loaded', function () {
    //Hide any loaders you have used to keep your user excited 
    //while their precious models are being processed and loaded
    //and start the animation.
    viewer.start();
    viewer.show(ViewType.DEFAULT);
});

viewer.on('error', function (arg) {
    var container = document.getElementById('errors');
    if (container) {
        //preppend error report
        container.innerHTML = "<pre style='color:red;'>" + arg.message + "</pre> <br />" + container.innerHTML;
    }
});

viewer.on('fps', function (fps) {
    var span = document.getElementById('fps');
    if (span) {
        span.innerHTML = fps;
    }
});

viewer.on('pick', function (args) {
    if (args == null || args.id == null) {```
        return;
    }You can inspect full code in [live example](3_Eventful_building.live.html).

    //you can use ID for funny things like hiding or
    //recolouring which will be covered in one of the next tutorials
    var id = args.id;
    var modelId = args.model;
    var coords = `[${Array.from(args.xyz).map(c => c.toFixed(2))}]`;

    document.getElementById('productId').innerHTML = id;
    document.getElementById('modelId').innerHTML = modelId;
    document.getElementById('coordsId').innerHTML = coords;
});

viewer.on('dblclick', function(args) {
    if(args == null || args.id == null) {
        return;
    }
    viewer.zoomTo(args.id);
});
```

So, let's have a look on the `loaded` event. It is the first one to occur 
and you are likely to use it to hide any loader images you may have used to keep users attention.
It is also better to start animation when model is loaded already. However, viewer won't crash
if you start it at any time. You should also set up the default view (left, right, top, bottom, etc.)

```js
viewer.on('loaded', function () {
    //Hide any loaders you have used to keep your user excited 
    //while their precious models are being processed and loaded
    //and start the animation.
    viewer.start();
    viewer.show(ViewType.DEFAULT);
});
```

`Error` is a very important event to listen to and you should listen to it 
all the time. You should still use try-catch statements but this might report some useful
information. It's up to you if you expose messages to user but you should watch it carefully.

```js
viewer.on('error', function (arg) {
    var container = document.getElementById('errors');
    if (container) {
        //preppend error report
        container.innerHTML = "<pre style='color:red;'>" + arg.message + "</pre> <br />" + container.innerHTML;
    }
});
```

`FPS` stands for 'frames per second'. This event is fired approximately every 0.5 second.
It is one of the most useful performance indicators. Viewer's animation loop it bound to refresh of the browser screen
so it won't usually exceed 60fps. If you get bellow about 15fps user experience starts to be a bit 'sluggish'.
This depends mainly on the size and complexity of the model and specific GPU capabilities of the client.

```js
viewer.on('fps', function (fps) {
    var span = document.getElementById('fps');
    if (span) {
        span.innerHTML = fps;
    }
});
```

`Pick` is probably the most important user interaction event. It happens every time
when user clicks on the area of `<canvas>`. It's argument contains product ID which you can use for things
like selection, restyling and other interactive operations. If user clicks out of the model ID is null. 
This event also contains information about 3D position of the interaction.

Appart from `pick`, you can use any standard canvas event and you will get our wrapper with product ID, model ID,
XYZ of the event in model space and the original event.

```js
viewer.on('pick', function (args) {
    if (args == null || args.id == null) {```
        return;
    }You can inspect full code in [live example](3_Eventful_building.live.html).

    //you can use ID for funny things like hiding or
    //recolouring which will be covered in one of the next tutorials
    var id = args.id;
    var modelId = args.model;
    var coords = `[${Array.from(args.xyz).map(c => c.toFixed(2))}]`;

    document.getElementById('productId').innerHTML = id;
    document.getElementById('modelId').innerHTML = modelId;
    document.getElementById('coordsId').innerHTML = coords;
});

viewer.on('dblclick', function(args) {
    if(args == null || args.id == null) {
        return;
    }
    viewer.zoomTo(args.id);
});
```