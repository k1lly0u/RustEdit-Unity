Sean's Water
By Sean Gause
2018, Version 1.0.1
~~~~~~~~~~~~~~~~~~~

Hey there! Thanks for downloading Sean's Water. I hope you enjoy it!
If you want to, you can donate any amount to my PayPal using the link provided in the downloaded folder.

Sean's Water is a lightweight water shader for use in the Unity Engine (made using Unity 2018 and ShaderForge)
The download folder should contain:
- a 'Shaders' folder
- a 'Textures' folder (which has a free seamless normal map that you can use)
- a url to my paypal
- this readme file
- an 'Images' folder that has pretty screenshots of my water ;)

~~~~~~~~~~~~~~~~~~~
SET-UP   
~~~~~~~~~~~~~~~~~~~
This is just in case you don't know how to set up a shader. It's pretty easy :)
1) Drag and drop one of the shaders from (SeansWater/Shaders) into your project. The differences between the two are explained in the "Properties" section below.
2) Create a new material by using right click > new > material
3) Under the material's shader dropdown, select "Custom > SeansWater"
4) Tweak the values to your liking

~~~~~~~~~~~~~~~~~~~
PROPERTIES
~~~~~~~~~~~~~~~~~~~
SeansWater 1.0.1 introduces a new Deferred version of the shader that is compatable with Screen-Space reflections. This is useful for interior environments.
However, the deferred version is not transparent at all. The forward version is! Both can be used with reflection probes.

The shaders include:
- Three normal map slots with adjustable movement speed, intensity, and tiling
- A glossyness slider
- A base color option for the surface
- A specular color option to make fine adjustments to reflections and reflectivity

The FORWARD version (because it has transparency) also has the following added options:
- A normal map slot with adjustable intensity, speed, and tiling (controls refraction)
- A depth / opacity slider to adjust how deep the water gets before it starts to get murky.

~~~~~~~~~~~~~~~~~~~
Basics
~~~~~~~~~~~~~~~~~~~
The water looks pretty bland without a skybox, so Iv'e included a couple links to free skybox sources online and on the Asset store
Make sure your "environment reflections" are set to "skybox" in the Lighting settings window (or you have a reflection probe set up, otherwise the water will be black)
Also, if the water is simply gone, make sure your camera is set to the Deferred rendering path :)

~~~~~~~~~~~~~~~~~~~
Thanks! Have fun!
~~~~~~~~~~~~~~~~~~~
Send me neat pictures of the water in your game using the reddit thread that I posted this on. Or ask questions if you're having trouble with anything!
~~~~~~~~~~~~~~~~~~~
(also, again, you can donate to my paypal if you like the shader. Thanks buddy)








