-- to run this example load 2 images:
-- 1: palmsGroup_groundOnly
-- 2: palmsGroup_ground

checkShadow = true

function CalcShadowFactor(gnd,shadow)
  f=0
  if gnd!=0 and gnd>shadow then f = (gnd-  shadow)/gnd*255 end
  return f
end
-- taken and translated from: https://stackoverflow.com/questions/13806483/increase-or-decrease-color-saturation/13807455#13807455
function RGBtoHSV(color)
	local r,g,b,h,s,v = 0,0,0,0,0,0
	r=color[1]
	g=color[2]
	b=color[3]
	local min = math.min(r,g,b)
	local max = math.max(r,g,b)
	
	v=max
	local delta=max-min
	if max ~= 0 then -- s
		s=delta/max
	else
		-- r = g = b = 0 // s = 0, v is undefined
		s=0
		h=-1
	end
	if r==max then
		h=(g-b)/delta -- between yellow & magenta
	elseif g==max then
		h=2+(b-r)/delta -- between cyan & yellow
	else
		h=4+(r-g)/delta -- between magenta & cyan
	end
	h=h*60
	if h<0 then
		h=h+360
	end
	if h~=h then -- isNaN(h)
		h=0
	end
	return {h,s,v}
end
function HSVtoRGB(color)
	local i,h,s,v,r,g,b = 0,0,0,0,0,0,0
	h=color[1]
	s=color[2]
	v=color[3]
	if s==0 then -- achromatic (grey)
		r,g,b=v,v,v
		return {r,g,b}
	end
	h=h/60 -- sector 0 to 5
	i=math.floor(h)
	f=h-i -- factorial part of h
	p=v*math.max(0,(1-s))
	q=v*math.max(0,(1-s*f))
	t=v*math.max(0,(1-s*(1-f)))
	if i==0 then
		r,g,b=v,t,p
	elseif i==1 then
		r,g,b=q,v,p
	elseif i==2 then
		r,g,b=p,v,t
	elseif i==3 then
		r,g,b=p,q,v
	elseif i==4 then
		r,g,b=t,p,v
	else -- i==5
		r,g,b=v,p,q
	end
	return {r,g,b}
end

rf = CalcShadowFactor(imagesPixels[1][2],imagesPixels[2][2])
gf = CalcShadowFactor(imagesPixels[1][3],imagesPixels[2][3])
bf = CalcShadowFactor(imagesPixels[1][4],imagesPixels[2][4])
af = (rf+gf+bf)/3
result[1] = af*0.95
result[2] = 0
result[3] = 0
result[4] = 0

function AlphaToColor(a,rgb)
  local r,g,b = rgb[1],rgb[2],rgb[3]
  if a<255 then
    r = rgb[1]/255*(255-a)
    g = rgb[2]/255*(255-a)
    b = rgb[3]/255*(255-a)
  end
  return {r,g,b}
end

if checkShadow then
  rgb=AlphaToColor(result[1],{imagesPixels[1][2],imagesPixels[1][3],imagesPixels[1][4]})
  sf = 1+af/255*0.25
  vf = 1+af/255*-0.1
  hsv=RGBtoHSV(rgb);
  hsv[2] = hsv[2]*sf;
  hsv[3] = hsv[3]*vf;
  rgb=HSVtoRGB(hsv);
  result[1]=255
  result[2]=math.min(255,rgb[1])
  result[3]=math.min(255,rgb[2])
  result[4]=math.min(255,rgb[3])
end

