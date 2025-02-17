﻿using blueapp.Models;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace blueapp.Data
{
    public class GraphDrawable : IDrawable
    {
        public List<OperationRecord>? Records { get; set; }
        private Color GraphColor { get; set; } = Color.FromArgb("#fff6ab"); // 그래프 색상 노란색
        private Color DateColor { get; set; } = Color.FromArgb("#e0d996");  // 날짜 색상 주황색

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float margin = 20;
            float width = dirtyRect.Width - 2 * margin;
            float height = dirtyRect.Height - 2 * margin - 30;  // 텍스트 표시 공간을 추가 확보
            if (Records == null || Records.Count < 1) return;

            // 범위를 0에서 100 사이로 고정
            float maxScore = 100;
            float minScore = 0;
            float range = maxScore - minScore;

            // 데이터 포인트 간의 최소 간격
            float minGap = 50; // 최소 50 픽셀 간격
            float xScale = Math.Max(minGap, width / (float)(Records.Count));    // 최신 값이 오른족에 배치되도록 수정
            //float xScale = Math.Max(minGap, width / Records.Count);               // 최신 값이 왼쪽에 배치

            PathF path = new PathF();
            for (int i = 0; i < Records.Count; i++)
            {
                float x = margin + i * xScale;      // x좌표를 minGap으로 간격을 조정
                float y = margin + ((maxScore - Records[i].Rate) / range) * height;

                if (i == 0)
                    path.MoveTo(x, y);
                else
                    path.LineTo(x, y);

                // 점수를 데이터 포인트 위에 표시
                canvas.FontSize = 12;
                canvas.FontColor = GraphColor;
                string scoreLabel = Records[i].Rate.ToString();
                canvas.DrawString(scoreLabel, x, y - 10, HorizontalAlignment.Center);

                // 점선을 직접 그리기
                float dotSpacing = 5;
                float dotLength = 3;
                float currentY = y;
                canvas.StrokeSize = 1;
                canvas.StrokeColor = GraphColor;

                while (currentY < height + margin * 1.5f)
                {
                    canvas.DrawLine(x, currentY, x, Math.Min(currentY + dotLength, height + margin * 1.5f));
                    currentY += dotLength + dotSpacing;
                }

                // RequestTime을 그래프 아래에 표시
                string timeLabel = Records[i].Timestamp.ToString("MM/dd");
                canvas.FontSize = 14;
                canvas.FontColor = DateColor;
                canvas.DrawString(timeLabel, x, height + margin * 1.5f + 10, HorizontalAlignment.Center);
            }

            // 데이터 포인트를 이어주는 선 그리기
            canvas.StrokeColor = GraphColor;
            canvas.StrokeSize = 2;
            canvas.DrawPath(path);
        }
    }
}
