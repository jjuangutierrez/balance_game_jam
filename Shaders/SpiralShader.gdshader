shader_type canvas_item;
uniform float progress : hint_range(0.0, 1.0); // 0 = nada cubierto, 1 = todo cubierto
uniform vec4 color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform int transition_type : hint_range(0, 3) = 0; // 0 = persiana vertical, 1 = persiana horizontal, 2 = fade, 3 = círculo

void fragment() {
    float transition_value = 0.0;
    
    // Tipo 0: Persiana vertical (de arriba a abajo)
    if (transition_type == 0) {
        transition_value = step(UV.y, progress);
    }
    // Tipo 1: Persiana horizontal (de izquierda a derecha)
    else if (transition_type == 1) {
        transition_value = step(UV.x, progress);
    }
    // Tipo 2: Fade simple
    else if (transition_type == 2) {
        transition_value = progress;
    }
    // Tipo 3: Círculo que se expande/contrae
    else if (transition_type == 3) {
        vec2 center = vec2(0.5, 0.5);
        float dist = distance(UV, center);
        transition_value = step(dist, progress);
    }
    
    COLOR = color;
    COLOR.a *= transition_value;
}