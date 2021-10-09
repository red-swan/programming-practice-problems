// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;


contract GasGasGas {
    
    uint public gasRemaining = 7; 
    
    function hitsTheSpot() public view returns(bool){
        return (gasRemaining % 200 == 0);
    }
    
    function gasUp() public {
        gasRemaining = gasleft();
    }
    
    function destroy() public {
        selfdestruct(payable(address(this)));
    }
}
