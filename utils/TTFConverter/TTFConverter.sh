fontNames="Arial.ttf comic.ttf cour.ttf framd.ttf lucida.ttf times.ttf verdana.ttf"
pointSize=22

alphabet="A B C D E F G H I J K L M N O P Q R S T U V W X Y Z"

mkdir Export

for font in $fontNames; do
  mkdir Export/$font
  for c in $alphabet; do
    convert -font Fonts/$font -pointsize $pointSize label:$c Export/$font/$c.png
  done
done