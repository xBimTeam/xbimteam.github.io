Hello building
==============

Live example [here](1_Hello_building.live.html).

In this tutorial you will learn how to create the most basic and straightforward viewer. 
It won't do anything except showing the building. It will only use the built-in navigation
of the viewer and will not respond to any events. You can have a look [here](1_Hello_building.live.html) for live example.

The simplest way to bring our viewer into your project is to use NPM package. Library is compiled as an UMD package, so you can use it directly or as a module.
Following examples will just reference it as a global object.

```
npm install @xbim/viewer
```

Now, let's dig into the code:

```html
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Hello building!</title>
    <script src="../js/xbim-viewer.js"></script>
</head>
<body>
    <canvas id="viewer" width="500" height="300"></canvas>
    <script type="text/javascript">
        var viewer = new Viewer('viewer');
        viewer.on('loaded', () => {
            viewer.show(ViewType.DEFAULT);
        });
        viewer.load('../data/SampleHouse.wexbim');
        viewer.start();
    </script>
</body>
</html>
```

Well, this was pretty easy wasn't it? We just referenced the *xbim-viewer.js* library, create the `Viewer` object
passing id of `<canvas>` element and start animation. This is it! Just make sure you are running from web server, not just 
as a local file because Viewer uses AJAX to fetch the wexBIM data and most of the browsers impose CORS restrictions even on local
HTML files. Also make sure you don't use IE less than 11 because you need to have support for WebGL. You will learn how to 
check prerequisites at the end of this tutorial. If it still doesn't work check your webserver is serving wexBIM file as a
static content. If you don't want to install webserver only because of this but you have Python installed,
you can just run `python -m SimpleHTTPServer 9000` in the folder. It will serve all the files as a static content which is all you need for this tutorial.

Right, this is about enough for the first tutorial. So if you feel fresh you can jump right into the next one where you will learn 
how to check that the browser is actually [able to render the model](safe-hello-building.html). It'll look the same as this example so you can have a look on [live
show here](1_Hello_building.live.html)