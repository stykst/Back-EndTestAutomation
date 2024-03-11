import { mathEnforcer } from "../problems/03. Math Enforcer.js";
import { expect } from 'chai';

describe('mathEnforcer', function() {
    describe('addFive', function() {
      it('should return undefined for non-number input', function() {
        expect(mathEnforcer.addFive('test')).to.be.undefined;
        expect(mathEnforcer.addFive(true)).to.be.undefined;
      });
  
      it('should add 5 to a positive number', function() {
        expect(mathEnforcer.addFive(7)).to.equal(12);
      });
  
      it('should add 5 to a negative number', function() {
        expect(mathEnforcer.addFive(-3)).to.equal(2);
      });
  
      it('should add 5 to a floating-point number within 0.01 precision', function() {
        expect(mathEnforcer.addFive(3.14)).to.be.closeTo(8.14, 0.01);
      });
    });
  
    describe('subtractTen', function() {
      it('should return undefined for non-number input', function() {
        expect(mathEnforcer.subtractTen('test')).to.be.undefined;
        expect(mathEnforcer.subtractTen(true)).to.be.undefined;
      });
  
      it('should subtract 10 from a positive number', function() {
        expect(mathEnforcer.subtractTen(15)).to.equal(5);
      });
  
      it('should subtract 10 from a negative number', function() {
        expect(mathEnforcer.subtractTen(-8)).to.equal(-18);
      });
  
      it('should subtract 10 from a floating-point number within 0.01 precision', function() {
        expect(mathEnforcer.subtractTen(7.99)).to.be.closeTo(-2.01, 0.01);
      });
    });
  
    describe('sum', function() {
      it('should return undefined for non-number input', function() {
        expect(mathEnforcer.sum('test', 5)).to.be.undefined;
        expect(mathEnforcer.sum(10, 'test')).to.be.undefined;
      });
  
      it('should return the sum of two positive numbers', function() {
        expect(mathEnforcer.sum(3, 7)).to.equal(10);
      });
  
      it('should return the sum of a positive and a negative number', function() {
        expect(mathEnforcer.sum(5, -3)).to.equal(2);
      });
  
      it('should return the sum of two floating-point numbers within 0.01 precision', function() {
        expect(mathEnforcer.sum(1.2, 3.4)).to.be.closeTo(4.6, 0.01);
      });
    });
  });
  