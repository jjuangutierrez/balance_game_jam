shader_type canvas_item;

uniform float progress : hint_range(0.0, 1.0); // 0 = nada cubierto, 1 = todo cubierto
uniform vec4 color : source_color = vec4(0.0, 0.0, 0.0, 1.0);

void fragment() {
    if (UV.y > progress) {
        discard;
    }
    COLOR = color;
}
