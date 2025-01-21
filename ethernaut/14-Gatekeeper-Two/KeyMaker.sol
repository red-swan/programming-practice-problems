// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;



contract KeyMaker {


    function abiEncode() public view returns(bytes memory){
        return abi.encodePacked(msg.sender);
    }

    function keccak(bytes memory _bytes) public pure returns(bytes32) {
        return keccak256(_bytes);
    }

    function keccakBytes8() public view returns(bytes8) {
        return bytes8(keccak256(abi.encodePacked(msg.sender)));
    }
    
    function keccakBytes64() public view returns(uint64) {
        return uint64(bytes8(keccak256(abi.encodePacked(msg.sender))));
    }

    function shift(bytes8 _gateKey) public view returns(uint64) {
        return uint64(bytes8(keccak256(abi.encodePacked(msg.sender)))) ^ uint64(_gateKey);
    }

    function keyFit(bytes8 _gateKey) public view returns(bool) {
        return uint64(bytes8(keccak256(abi.encodePacked(msg.sender)))) ^ uint64(_gateKey) == uint64(0) - 1;
    }

    

}