function generateEmployeeList(employeeNames) {
    const employeeList = {};
  
    for (const name of employeeNames) {
      const personalNum = name.length;
      const [firstName, lastName] = name.split(' ');
      const employee = {
        firstName,
        lastName,
        personalNum
      };
  
      employeeList[name] = employee;
    }
  
    for (const [fullName, employee] of Object.entries(employeeList)) {
      console.log(`Name: ${fullName} -- Personal Number: ${employee.personalNum}`);
    }
  }