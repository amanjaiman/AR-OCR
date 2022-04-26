import cv2
import pytesseract
import sys

pytesseract.pytesseract.tesseract_cmd = 'C:\\Program Files\\Tesseract-OCR\\tesseract.exe'
custom_oem_psm_config = r'--oem 3 --psm 6'

def main(filename):
    img = cv2.imread(filename)

    img_rgb = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
    return pytesseract.image_to_string(img_rgb, config=custom_oem_psm_config)

if __name__ == '__main__':
    if len(sys.argv) != 2:
        print('Usage: python ocr.py image.jpg')
        sys.exit(1)
    
    filename = sys.argv[1]
    print(main(filename))