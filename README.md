```
Author:     Jeanmarco Allain

```

# Overview of functionality:
Originally built for a friend, this code converts the JSON files used in Genshin Impact to .fx files 
to be used in the MMDGenshin shader by Manashiku. 

# How to use:
Place the .json files to convert in the same directory as the Converter.exe executable. 
Launch the executable and type in the name of the JSON file to convert. 

# Optional features:
*Use Normals:* Decide whether or not to use normals in the .fx file. Please note that current functionality
does not contain a normal texture file path in the outputted .fx file. (Default is to not use normals)

*Outline Multiplier:* For some reason, the raw JSON's outline size is very thin. It is possible that in-game
some additional multiplication or math is done to this raw value. Regardless, you may specify a multiplier float
to increase the size of the outline. (Eyeballing it, 8 seems to be a reasonable multiplier. Default is 1)

# Final notes:
You may want to consider editing the outputted .fx files to your liking of course. The raw numbers alone converted
to the .fx files may look off without additional tweaking.

Some things are hardcoded, such as the base material values which all seem to be (1.0F, 1.0F, 1.0F, 1.0F), the paths
to the /sub/tex files in the shader, and the fact that "define USE_LIGHTMAP_AO", "define USE_VERTEXCOLOR_AO", and 
"define USE_VERTEXCOLOR_RAMP_WIDTH" are all enabled regardless of JSON data. Finally, this code uses only the 
Girl_Tex_FaceLightmap to drive .fx face light map option. 

Download the source code if needed and feel free to implement changes. (Consider a PR, if you want.)

To respect intellectual property, the JSON files will *not* be provided. *Do not ask.* 
You can extract them from the game yourself or find them online.

# Additional reading:
https://github.com/Manashiku/MMDGenshin/wiki
https://docs.google.com/document/d/1AxOFEiPqNvBOYcR5RFI4UzHg6z9tr8Ex7U1lIBVXFwU/edit

Thank you to @Manashiku https://twitter.com/manashiku and @MonoCereal https://twitter.com/monocereal

