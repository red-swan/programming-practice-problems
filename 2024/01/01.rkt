#lang racket

(require
  relation/type
  seq)

; function definitions ------------------------------------------------

(define (find-diff xs ys)
  (for/foldr ([acc 0]
              #:result acc)
             ([x xs]
              [y ys])
    (+ acc (abs (- y x)))))

(define (find-sim xs ys [acc 0])
  ; if we're out of items, return the accumulator
  (if (or (empty? xs) (empty? ys))
      acc
      ; otherwise, get the heads
      (let* ([x (first xs)]
             [y (first ys)])
        (cond
          ; if lhs is less, then skip it
          [(< x y) (find-sim (rest xs) ys acc)]
          ; if rhs is less, then skip it
          [(< y x) (find-sim xs (rest ys) acc)]
          ; if equal, move rhs down one
          [else (find-sim xs (rest ys) (+ acc x))]))))

(define (split-pairs line)
  (map string->number (regexp-match* #px"(\\d+)" line)))    


; reading the data ----------------------------------------------------
(define-values (lhs rhs)
  (let* ([zipped-nums (with-input-from-file "01.txt"
                        (λ ()
                          (for/list ([line (in-lines)])
                            (split-pairs line))))]
         [list-of-lists (map ->list (unzip zipped-nums))])
    (apply values list-of-lists)))


; computing the answers -----------------------------------------------

(let* ([sortedLhs (sort lhs <)]
       [sortedRhs (sort rhs <)]
       [part1 (find-diff sortedLhs sortedRhs)]
       [part2 (find-sim sortedLhs sortedRhs)])
  (begin
    (displayln (format "Part 1: ~a" part1))
    (displayln (format "Part 2: ~a" part2))))