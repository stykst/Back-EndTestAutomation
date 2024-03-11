import { isSymmetric } from "../problems/02. Check for Symmetry.js";
import { expect } from 'chai';

describe('isSymmetric', function() {
    it('should return true for a symmetric array', function() {
      expect(isSymmetric([1, 2, 3, 3, 2, 1])).to.be.true;
    });
  
    it('should return true for an empty array', function() {
      expect(isSymmetric([])).to.be.true;
    });
  
    it('should return true for an array with a single element', function() {
      expect(isSymmetric([5])).to.be.true;
    });
  
    it('should return true for an array with repeated elements', function() {
      expect(isSymmetric(['a', 'b', 'c', 'c', 'b', 'a'])).to.be.true;
    });
  
    it('should return false for a non-symmetric array', function() {
      expect(isSymmetric([1, 2, 3, 4, 5])).to.be.false;
    });
  
    it('should return false for null input', function() {
      expect(isSymmetric(null)).to.be.false;
    });
  
    it('should return false for undefined input', function() {
      expect(isSymmetric(undefined)).to.be.false;
    });
    it('should return true for an empty array', function() {
      expect(isSymmetric([])).to.be.true;
    });
  
    it('should return false for a non-symmetric array', function() {
      expect(isSymmetric([1, 2, 3, 4, 5])).to.be.false;
    });
  
    it('should return false for a non number symmetric array', function() {
      expect(isSymmetric(['1', '2', '3', 2, 1])).to.be.false;
    });
  
    it('should return true for an array with a single element', function() {
      expect(isSymmetric([5])).to.be.true;
    });
  
    it('should return true for an array with repeated elements', function() {
      expect(isSymmetric(['a', 'b', 'c', 'c', 'b', 'a'])).to.be.true;
    });
  });