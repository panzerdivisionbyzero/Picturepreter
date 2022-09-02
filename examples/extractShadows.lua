-- instructions to run this script:
-- 1. Load images:
-- - palmsGroup_groundOnly.bmp
-- - palmsGroup_ground.bmp
-- 2. Check "Result image" radio button
-- Optional: comment out or set "transparencyToColor" to "false"

transparencyToColor = true

function CalcShadowFactor(gnd,shadow)
  f=0
  if gnd!=0 and gnd>shadow then f = (gnd-shadow)/gnd*255 end
  return f
end

rf = CalcShadowFactor(imagesPixels[1][2],imagesPixels[2][2])
gf = CalcShadowFactor(imagesPixels[1][3],imagesPixels[2][3])
bf = CalcShadowFactor(imagesPixels[1][4],imagesPixels[2][4])
af = (rf+gf+bf)/3
result = {af,0,0,0}

if transparencyToColor then
  function TransparencyToColor(r,g,b)
    if result[1]<255 then
      result[2] = r/255*(255-result[1])
      result[3] = g/255*(255-result[1])
      result[4] = b/255*(255-result[1])
    end
    result[1] = 255
  end
  c = 128
  TransparencyToColor(c,c,c)
end