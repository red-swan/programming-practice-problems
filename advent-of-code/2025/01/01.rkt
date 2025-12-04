#lang racket


(define (turn dir from)
  (match-let ([(list _ dir amount) (regexp-match #px"^(L|R)(\\d+)" dir)])
    (let ([amount (string->number amount)]
          [f (if (equal? dir "L") - +)])
      (modulo (f from amount) 100))))


(define turns (file->lines "input.txt"))
(define codes
  (for/fold ([acc '(50)]
             [x 50]
             #:result (reverse acc))
            ([instruction turns])
    (let ([x* (turn instruction x)])
      (values (cons x* acc) x*))))


; Part 1
(count zero? codes)
