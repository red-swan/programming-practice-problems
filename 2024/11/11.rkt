#lang racket
k
; returns the number of digits in a base 10 number
(define (digits n)
  (add1 (exact-floor (log n 10))))

; returns true if a number has an even number of digits
(define (even-digits? n)
  (even? (digits n)))

; closure to create generic increment functions
(define (add y)
  (λ (x) (+ x y)))
 
; splits a number in two parts
; does not check if the number has an even number of digits
(define (split-digits x)
  (define (find-divisor x b d)
    (if (< d (quotient x d))
        (find-divisor x b (* d b))
        d))
  (let-values ([(q r) (quotient/remainder x (find-divisor x 10 10))])
    (list q r)))

; blink a single number into a list of numbers
(define (blink x)
  (match x
    [0  (list 1)]
    [_  #:when (even-digits? x) (split-digits x)]
    [_ (list (* x 2024))]))

; convert a list of stones to a mapping of their counts
(define (stones->map stones)
  (for/fold ([acc (hash)])
            ([s stones])
    (hash-update acc s add1 0)))

; blink a stone-count mapping to the get the count mapping
; for the next level
(define (blink-map stone-map)
  ; blink each stone at the level we're at
  (for/fold ([stone-map* (hash)])
            ([(stone count) (in-hash stone-map)])
    ; for each stone in the blink list
    (for/fold ([m stone-map*])
              ([num (blink stone)])
      (hash-update m num (add count) 0))))

; function to count the stones after blinking n times
(define (count stones n)
  (let* ([stone-mapN (for/fold ([acc (stones->map stones)]) [(x (in-range n))] (blink-map acc))])
    ; sum up the counts of all numbers
    (for/sum ([(stone count) (in-hash stone-mapN)]) count)))

; Problem input -------------------------------------------------------
(define stones '(9694820 93 54276 1304 314 664481 0 4))

; Part 1 --------------------------------------------------------------
(displayln (format "Part 1: ~a" (count stones 25)))

; Part 2 --------------------------------------------------------------
(displayln (format "Part 2: ~a" (count stones 75)))
