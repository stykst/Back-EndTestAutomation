import { analyzeArray } from "../problems/04. Array Analyzer.js";
import { expect } from 'chai';

describe('analyzeArray function', () => {
    it('should return the correct result for an array of numbers', () => {
      const result = analyzeArray([3, 1, 7, 4, 2]);
      expect(result).to.deep.equal({ min: 1, max: 7, length: 5 });
    });
  
    it('should return undefined for an empty array', () => {
      const result = analyzeArray([]);
      expect(result).to.be.undefined;
    });
  
    it('should return undefined for a non-array input', () => {
      const result = analyzeArray('not an array');
      expect(result).to.be.undefined;
    });
  
    it('should return the correct result for a single element array', () => {
      const result = analyzeArray([5]);
      expect(result).to.deep.equal({ min: 5, max: 5, length: 1 });
    });
  
    it('should return the correct result for an array with equal elements', () => {
      const result = analyzeArray([2, 2, 2, 2]);
      expect(result).to.deep.equal({ min: 2, max: 2, length: 4 });
    });
  
    it('should handle arrays with negative numbers', () => {
      const result = analyzeArray([-1, -5, -3, -2]);
      expect(result).to.deep.equal({ min: -5, max: -1, length: 4 });
    });
  
    it('should handle arrays with floating-point numbers', () => {
      const result = analyzeArray([2.5, 1.2, 3.8, 1.5]);
      expect(result).to.deep.equal({ min: 1.2, max: 3.8, length: 4 });
    });
  });
  