import { sum } from "../problems/01. Sum of Numbers.js";
import { expect } from 'chai';

describe('sum', function() {
    it('should return 0 for an empty array', function() {
      expect(sum([])).to.equal(0);
    });
  
    it('should return the correct sum for an array of positive numbers', function() {
      expect(sum([1, 2, 3])).to.equal(6);
    });
  
    it('should return the correct sum for an array of negative numbers', function() {
      expect(sum([-1, -2, -3])).to.equal(-6);
    });
  
    it('should return the correct sum for an array with mixed positive and negative numbers', function() {
      expect(sum([1, -2, 3])).to.equal(2);
    });
  
    it('should return the correct sum for an array with floating-point numbers', function() {
      expect(sum([1.5, 2.5, 3.5])).to.equal(7.5);
    });
  
    it('should return the correct sum for an array with a single number', function() {
      expect(sum([5])).to.equal(5);
    });
  
    it('should return NaN if the array contains non-numeric elements', function() {
      expect(sum([1, 'a', 3])).to.be.NaN;
    });
  });