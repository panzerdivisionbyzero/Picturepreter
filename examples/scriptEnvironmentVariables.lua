-- before running script load some image(s) to determine result image size
threshold = 0.02
function UpperLine() return currentY/imageH<threshold end
function LeftLine() return currentX/imageW<threshold end
function BottomLine() return (imageH-currentY)/imageH<threshold end
function RightLine() return (imageW-currentX)/imageW<threshold end
function BackSlash() return math.abs(currentX/imageW-currentY/imageH)<threshold end
function Slash() return math.abs((imageW-currentX)/imageW-currentY/imageH)<threshold end
function ChunkBorder()
	return currentX==chunkStartX or currentX==chunkLastX or currentY==chunkStartY or currentY==chunkLastY
end

if UpperLine() or LeftLine() or BottomLine() or RightLine() then
  result[1] = 255
  result[2] = 0
  result[3] = 0
  result[4] = 255
elseif BackSlash() or Slash() then
  result[1] = 128
  result[2] = 255
  result[3] = 0
  result[4] = 0
elseif imagesCount>0 then
  result[1] = 64
  result[2] = imagesPixels[1][2]
  result[3] = imagesPixels[1][3]
  result[4] = imagesPixels[1][4]
else
  result[1] = 0
  result[2] = 255
  result[3] = 255
  result[4] = 255
end

-- The script environment provides global variables and functions:
-- - imagesPixels[imageIndex] = {A,R,G,B}
-- - result = {A,R,G,B}
-- (for read/use only purposes):
-- - chunkStartX
-- - chunkStartY
-- - chunkLastX
-- - chunkLastY
-- - imageW
-- - imageH
-- - currentX
-- - currentY
-- - imagesCount
-- - CastToByte(int) // returns "byte" (int within range: [0..255])
-- - TableLength(tab) // returns int
-- - DebugEachPx(str) // prints given string into output text box
-- - DebugForPx(x,y,str)
-- - DebugChunkBegin(str)
-- - DebugChunkEnd(str)
-- - DebugImageBegin(str)
-- - DebugImageEnd(str)