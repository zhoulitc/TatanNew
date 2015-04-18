var vg = '0';
(function () {
    var v1 = '1';
    if (v1 === '3') {
        var v2 = '2';
    }
    console.log(vg); //scope chain:vg = '0'
    console.log(v1);
    console.log(v2); //hoisting:v2 = undefined
})();
console.log(v1); //scope error，不存在v1

var ceo = {
    name: "ceo",
    doing: function () {
        console.log(this.name);
    }
};
var loser = {
    name: "loser",
    doing: ceo.doing
};
loser.doing(); //this: name = 'loser'




var a = {}, b = {}, c = {};
b.__proto__ = a; //a.__proto__ = [object],[object].__proto__ = null
c.__proto__ = b;
a.t = 3;
console.log(b.t); //prototype: b.t=3
console.log(b.t); //prototype chain: c.t=3
b.t = 4;
console.log(a.t); //a.t=3
console.log(b.t); //b.t=4
console.log(c.t); //c.t=4
console.log(c.r); //undefined