-- instructions to run this script:
-- 1. Load images:
-- - palmsGroup_ground.bmp
-- - palmsGroup_top.bmp
-- 2. Check "Result image preview" radio button

if imagesPixels[2][2]==88 and imagesPixels[2][3]==0 and imagesPixels[2][4]==0 then
  result[2]=imagesPixels[1][2]
  result[3]=imagesPixels[1][3]
  result[4]=imagesPixels[1][4]
else
  result[2]=imagesPixels[2][2]
  result[3]=imagesPixels[2][3]
  result[4]=imagesPixels[2][4]
end

result[1]=255
