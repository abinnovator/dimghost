# SPDX-License-Identifier: MIT
extends Node3D
@onready var camera: Camera3D = $Camera3D
var yaw := 0.60
var pitch := 0.48
var distance := 15.0
var focus := Vector3(0, 1.2, 0)
var dragging := false
func _ready() -> void:
    yaw = get_meta("yaw", 0.60)
    pitch = get_meta("pitch", 0.48)
    distance = get_meta("distance", 15.0)
    focus = get_meta("focus", Vector3(0, 1.2, 0))
    update_camera()
func update_camera() -> void:
    camera.position = focus + Vector3(sin(yaw)*cos(pitch), sin(pitch), cos(yaw)*cos(pitch))*distance
    camera.look_at(focus)
    camera.size = distance * 0.82
func _unhandled_input(event: InputEvent) -> void:
    if event is InputEventMouseButton:
        if event.button_index == MOUSE_BUTTON_LEFT: dragging = event.pressed
        if event.pressed and event.button_index == MOUSE_BUTTON_WHEEL_UP: distance = maxf(2.0, distance*.90)
        if event.pressed and event.button_index == MOUSE_BUTTON_WHEEL_DOWN: distance = minf(40.0, distance*1.10)
    elif event is InputEventMouseMotion and dragging:
        yaw -= event.relative.x*.006
        pitch = clampf(pitch+event.relative.y*.006,.08,1.50)
    elif event is InputEventKey and event.pressed:
        if event.keycode == KEY_R: _ready()
        if event.keycode == KEY_H and has_node("Roof"): $Roof.visible = not $Roof.visible
        if event.keycode == KEY_TAB:
            var demos: Array[String] = ["DemoRoom", "Exterior", "Showcase"]
            get_tree().change_scene_to_file("res://addons/kominka_modular_home/demo/"+demos[(demos.find(str(name))+1)%demos.size()]+".tscn")
    update_camera()
