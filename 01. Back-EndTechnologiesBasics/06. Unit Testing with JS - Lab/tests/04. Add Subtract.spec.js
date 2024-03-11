import { createCalculator } from "../problems/04. Add Subtract.js";
import { expect } from 'chai';

describe('createCalculator', function() {
    it('should return an object with add, subtract, and get functions', function() {
      const calculator = createCalculator();
      expect(calculator).to.be.an('object');
      expect(calculator).to.have.property('add').to.be.a('function');
      expect(calculator).to.have.property('subtract').to.be.a('function');
      expect(calculator).to.have.property('get').to.be.a('function');
    });
  
    it('should initially have a sum of 0', function() {
      const calculator = createCalculator();
      expect(calculator.get()).to.equal(0);
    });
  
    it('should add numbers correctly', function() {
      const calculator = createCalculator();
      calculator.add(5);
      expect(calculator.get()).to.equal(5);
  
      calculator.add(10);
      expect(calculator.get()).to.equal(15);
    });
  
    it('should handle adding strings containing numbers', function() {
      const calculator = createCalculator();
      calculator.add('5');
      expect(calculator.get()).to.equal(5);
  
      calculator.add('10');
      expect(calculator.get()).to.equal(15);
    });
  
    it('should subtract numbers correctly', function() {
      const calculator = createCalculator();
      calculator.subtract(5);
      expect(calculator.get()).to.equal(-5);
  
      calculator.subtract(10);
      expect(calculator.get()).to.equal(-15);
    });
  
    it('should handle subtracting strings containing numbers', function() {
      const calculator = createCalculator();
      calculator.subtract('5');
      expect(calculator.get()).to.equal(-5);
  
      calculator.subtract('10');
      expect(calculator.get()).to.equal(-15);
    });
  
    it('should handle mixed addition and subtraction', function() {
      const calculator = createCalculator();
      calculator.add(5);
      calculator.subtract(2);
      calculator.add(10);
      expect(calculator.get()).to.equal(13);
    });
  
    it('should handle invalid input for add', function() {
      const calculator = createCalculator();
      calculator.add('invalid');
      expect(calculator.get()).to.be.NaN;
    });
  
    it('should handle invalid input for subtract', function() {
      const calculator = createCalculator();
      calculator.subtract('invalid');
      expect(calculator.get()).to.be.NaN;
    });
  });
  