import { isOddOrEven } from "../problems/01. Even Or Odd.js";
import { expect } from 'chai';

describe('isOddOrEven', () => {
    it('should return undefined for non-string input', () => {
      expect(isOddOrEven(15)).to.be.undefined;
      expect(isOddOrEven(10.10)).to.be.undefined;
      expect(isOddOrEven(null)).to.be.undefined;
      expect(isOddOrEven(undefined)).to.be.undefined;
    });
  
    it('should return even for a string with even length', () => {
      expect(isOddOrEven('1234')).to.be.equal('even');
    });
  
    it('should return odd for a string with odd length', () => {
      expect(isOddOrEven('123')).to.be.equal('odd');
    });
  
    it('should work correctly with multiple different strings', () => {
      expect(isOddOrEven('even')).to.be.equal('even');
      expect(isOddOrEven('odd')).to.be.equal('odd');
      expect(isOddOrEven('')).to.be.equal('even');
      expect(isOddOrEven('fun')).to.be.equal('odd');
      expect(isOddOrEven('test')).to.be.equal('even');
    });
  });
  