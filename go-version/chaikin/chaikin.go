package chaikin

import (
	rl "github.com/gen2brain/raylib-go/raylib"
)

func ChaikinSmooth(points []rl.Vector2) []rl.Vector2 {
	result := make([]rl.Vector2, 0);

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
