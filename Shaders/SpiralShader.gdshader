shader_type canvas_item;

uniform float progress : hint_range(0.0, 1.0);
uniform vec4 color : source_color = vec4(0.0, 0.0, 0.0, 1.0);

void fragment() {
    vec2 uv = UV * 2.0 - 1.0; // UV centrado
    float radius = length(uv);
    float angle = atan(uv.y, uv.x); // ángulo en radianes

    float spiral_val = radius * 3.0 + angle / (2.0 * PI); // 3.0 controla la densidad
    if (spiral_val > progress * 3.0) {
        discard;
    }

    COLOR = color;
}
