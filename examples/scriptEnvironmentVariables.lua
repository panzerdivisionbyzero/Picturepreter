threshold = 0.02
function UpperLine() return imageY/imageH<threshold end
function LeftLine() return imageX/imageW<threshold end
function BottomLine() return (imageH-imageY)/imageH<threshold end
function RightLine() return (imageW-imageX)/imageW<threshold end
function BackSlash() return math.abs(imageX/imageW-imageY/imageH)<threshold end
function Slash() return math.abs((imageW-imageX)/imageW-imageY/imageH)<threshold end

if UpperLine() or LeftLine() or BottomLine() or RightLine() then
  resultA = 255
  resultR = 0
  resultG = 0
  resultB = 255
elseif BackSlash() or Slash() then
  resultA = 128
  resultR = 255
  resultG = 0
  resultB = 0
else
  resultA = 0
  resultR = 255
  resultG = 255
  resultB = 255
end

