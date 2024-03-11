import { rgbToHexColor } from "../problems/03. RGB to Hex.js";
import { expect } from 'chai';

describe('rgbToHexColor', function() {
    it('should convert RGB values to hex color', function() {
      expect(rgbToHexColor(255, 0, 0)).to.equal('#FF0000');
    });
  
    it('should handle minimum RGB values', function() {
      expect(rgbToHexColor(0, 0, 0)).to.equal('#000000');
    });
  
    it('should handle maximum RGB values', function() {
      expect(rgbToHexColor(255, 255, 255)).to.equal('#FFFFFF');
    });
  
    it('should convert and format hex color correctly', function() {
      expect(rgbToHexColor(12, 34, 56)).to.equal('#0C2238');
    });
  
    it('should return undefined for non-integer red value', function() {
      expect(rgbToHexColor('red', 0, 0)).to.be.undefined;
    });
  
    it('should return undefined for red value below 0', function() {
      expect(rgbToHexColor(-1, 0, 0)).to.be.undefined;
    });
  
    it('should return undefined for red value above 255', function() {
      expect(rgbToHexColor(256, 0, 0)).to.be.undefined;
    });
  
    it('should return undefined for non-integer green value', function() {
      expect(rgbToHexColor(0, 'green', 0)).to.be.undefined;
    });
  
    it('should return undefined for green value below 0', function() {
      expect(rgbToHexColor(0, -1, 0)).to.be.undefined;
    });
  
    it('should return undefined for green value above 255', function() {
      expect(rgbToHexColor(0, 256, 0)).to.be.undefined;
    });
  
    it('should return undefined for non-integer blue value', function() {
      expect(rgbToHexColor(0, 0, 'blue')).to.be.undefined;
    });
  
    it('should return undefined for blue value below 0', function() {
      expect(rgbToHexColor(0, 0, -1)).to.be.undefined;
    });
  
    it('should return undefined for blue value above 255', function() {
      expect(rgbToHexColor(0, 0, 256)).to.be.undefined;
    });
  
    it('should return undefined for invalid types for all RGB values', function() {
      expect(rgbToHexColor('red', 'green', 'blue')).to.be.undefined;
    });
  });
  