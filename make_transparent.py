from PIL import Image

input_path = "Assets/winnie.png"
output_path = "Assets/winnie2.png"

image = Image.open(input_path).convert("RGBA")
pixels = image.load()

for y in range(image.height):
    for x in range(image.width):
        r, g, b, a = pixels[x, y]

        if r == 255 and g == 255 and b == 255:
            pixels[x, y] = (255, 255, 255, 0)

image.save(output_path)