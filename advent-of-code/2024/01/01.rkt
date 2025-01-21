#lang racket


; reading the data ----------------------------------------------------
(define (split-pairs line)
  (map string->number (regexp-match* #px"(\\d+)" line)))

(define (transpose xss)
  (apply map list xss))

(define-values (lhs rhs)
  (let* ([zipped-nums   (with-input-from-file "01.txt"
                          (λ ()
                            (for/list ([line (in-lines)])
                              (split-pairs line))))])
    (apply values (transpose zipped-nums))))


; function definitions ------------------------------------------------

(define (find-diff xs ys)
  (for/foldr ([acc 0]
              #:result acc)
             ([x xs]
              [y ys])
    (+ acc (abs (- y x)))))

(define (build-counts xs)
  (for/fold ([counts (hash)])
            ([x xs])
      (hash-update counts x add1 0)))

(define (find-sim xs ys [acc 0])
  (let ([yCounts (build-counts ys)])
    (for/fold ([similarity 0])
              ([x xs])
      (+ similarity (* x (hash-ref yCounts x 0))))))


; computing the answers -----------------------------------------------

(let* ([sortedLhs (sort lhs <)]
       [sortedRhs (sort rhs <)]
       [part1 (find-diff sortedLhs sortedRhs)]
       [part2 (find-sim sortedLhs sortedRhs)])
  (begin
    (displayln (format "Part 1: ~a" part1))
    (displayln (format "Part 2: ~a" part2))))
