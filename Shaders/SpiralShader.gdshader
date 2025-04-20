shader_type canvas_item;
uniform float progress : hint_range(0.0, 1.0);
uniform vec4 color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform int transition_type : hint_range(0, 3) = 0;

void fragment() {
    float transition_value = 0.0;
    
    if (transition_type == 0) {
        transition_value = step(UV.y, progress);
    }
    else if (transition_type == 1) {
        transition_value = step(UV.x, progress);
    }
    else if (transition_type == 2) {
        transition_value = progress;
    }
    else if (transition_type == 3) {
        vec2 center = vec2(0.5, 0.5);
        float dist = distance(UV, center);
        transition_value = step(dist, progress);
    }
    
    COLOR = color;
    COLOR.a *= transition_value;
}