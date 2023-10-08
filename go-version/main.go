package main

import (
	"image/color"
	"math/rand"
	"visualizer/chaikin"

	rl "github.com/gen2brain/raylib-go/raylib"
)

var (
	points = []rl.Vector2{
		{X: float32(rand.Intn(800)), Y: float32(rand.Intn(800))},
		{X: float32(rand.Intn(800)), Y: float32(rand.Intn(800))},
		{X: float32(rand.Intn(800)), Y: float32(rand.Intn(800))},
		{X: float32(rand.Intn(800)), Y: float32(rand.Intn(800))},
	}
)

func main() {
	divisionCount := 7

	rl.SetConfigFlags(rl.FlagWindowHighdpi | rl.FlagMsaa4xHint)

	rl.InitWindow(800, 800, "visualizer")

	rl.SetTargetFPS(1)

	for !rl.WindowShouldClose() {
		rl.BeginDrawing()
		rl.ClearBackground(rl.Black)

		// Smoothed lines
		newPoints := points
		for i := 0; i < divisionCount; i++ {
			newPoints = chaikin.ChaikinSmooth(newPoints)
		}
		for i := 0; i < len(newPoints)-1; i++ {
			rl.DrawLineEx(
				rl.Vector2{X: newPoints[i].X, Y: newPoints[i].Y},
				rl.Vector2{X: newPoints[i+1].X, Y: newPoints[i+1].Y},
				2,
				color.RGBA{255, 255, 255, 255},
			)
		}

		rl.EndDrawing()

		divisionCount++
		if divisionCount > 7 {
			divisionCount = 7
			points = []rl.Vector2{}
			n := rand.Intn(5) + 4
			for i := 0; i < n; i++ {
				points = append(points, rl.Vector2{X: float32(rand.Intn(800)), Y: float32(rand.Intn(800))})
			}
		}
	}

	rl.CloseWindow()
}