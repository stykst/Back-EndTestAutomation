
import { lookupChar } from "../problems/02. Char Lookup.js";
import { expect } from 'chai';

describe('lookupChar', function() {
    it('should return undefined for non-string first parameter', function() {
      expect(lookupChar(123, 0)).to.be.undefined;
      expect(lookupChar(true, 0)).to.be.undefined;
      expect(lookupChar(['a', 'b', 'c'], 0)).to.be.undefined;
    });
  
    it('should return undefined for non-integer second parameter', function() {
      expect(lookupChar('test', '0')).to.be.undefined;
      expect(lookupChar('test', 0.5)).to.be.undefined;
      expect(lookupChar('test', true)).to.be.undefined;
    });
  
    it('should return "Incorrect index" for negative index', function() {
      expect(lookupChar('test', -1)).to.equal('Incorrect index');
    });
  
    it('should return "Incorrect index" for index equal to string length', function() {
      expect(lookupChar('test', 4)).to.equal('Incorrect index');
    });
  
    it('should return "Incorrect index" for index greater than string length', function() {
      expect(lookupChar('test', 5)).to.equal('Incorrect index');
    });
  
    it('should return correct character for valid parameters', function() {
      expect(lookupChar('test', 0)).to.equal('t');
      expect(lookupChar('test', 1)).to.equal('e');
      expect(lookupChar('test', 2)).to.equal('s');
      expect(lookupChar('test', 3)).to.equal('t');
    });
  });
  