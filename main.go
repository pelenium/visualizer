package main

import (
	"image/color"

	rl "github.com/gen2brain/raylib-go/raylib"
)

var (
	points = []rl.Vector2{
		{X: 40, Y: 40},
		{X: 200, Y: 130},
		{X: 300, Y: 360},
		{X: 500, Y: 340},
	}
)

func chaikinSmooth(points []rl.Vector2) []rl.Vector2 {
	result := make([]rl.Vector2, 0)

	result = append(result, points[0])
	for i := 0; i < len(points)-1; i++ {
		prev := points[i]
		next := points[i+1]

		a := rl.Vector2{
			X: prev.X*3/4 + next.X*1/4,
			Y: prev.Y*3/4 + next.Y*1/4,
		}

		b := rl.Vector2{
			X: prev.X*1/4 + next.X*3/4,
			Y: prev.Y*1/4 + next.Y*3/4,
		}

		result = append(result, a)
		result = append(result, b)
	}
	result = append(result, points[len(points)-1])

	return result
}

func main() {
	divisionCount := 3

	rl.SetConfigFlags(rl.FlagWindowHighdpi | rl.FlagMsaa4xHint)

	rl.InitWindow(800, 800, "visualizer")

	rl.SetTargetFPS(1)

	for !rl.WindowShouldClose() {
		rl.BeginDrawing()

		rl.ClearBackground(rl.Black)

		// Линии между точками
		for i := 0; i < len(points)-1; i++ {
			rl.DrawLine(
				int32(points[i].X), int32(points[i].Y),
				int32(points[i+1].X), int32(points[i+1].Y),
				color.RGBA{255, 255, 255, 100},
			)
		}

		// Сглаженные линии
		newPoints := points
		for i := 0; i < divisionCount; i++ {
			newPoints = chaikinSmooth(newPoints)
		}
		for i := 0; i < len(newPoints)-1; i++ {
			rl.DrawLineEx(
				rl.Vector2{X: newPoints[i].X, Y: newPoints[i].Y},
				rl.Vector2{X: newPoints[i+1].X, Y: newPoints[i+1].Y},
				2,
				color.RGBA{255, 255, 255, 255},
			)
		}

		// Сглаженные точки
		for _, point := range newPoints {
			rl.DrawCircle(int32(point.X), int32(point.Y), 5, rl.Orange)
		}

		// Базовые точки
		for _, point := range points {
			rl.DrawCircle(int32(point.X), int32(point.Y), 7, rl.Red)
		}

		rl.EndDrawing()

		divisionCount++
		if divisionCount > 5 {
			divisionCount = 0
		}
	}

	rl.CloseWindow()
}
