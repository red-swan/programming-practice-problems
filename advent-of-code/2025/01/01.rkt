#lang racket


; Functions -------------------------------------------------------------

(define (string->instruction s)
  (match-let ([(list _ dir amount) (regexp-match #px"^(L|R)(\\d+)" s)])
    (list
     (if (equal? dir "L") 'L 'R)
     (string->number amount))))


(define (count-zeroes tick dir amount)
  (let-values ([(q r) (quotient/remainder amount 100)])
    (if (and
         ((negate zero?) tick)
         (or
          (and (equal? 'L dir)
               (<= tick r))
          (and (equal? 'R dir)
               (<= (- 100 tick) r))))
        (add1 q)
        q)))

(define (turn tick dir amount)
  (let* ([f (case dir ['L -] ['R +])]
         [result (f tick amount)]
         [tick* (modulo result 100)])
    (values
     ; new tick
     tick*
     ; tick* on zero?
     (if (zero? tick*) 1 0)
     ; touched zero?
     (count-zeroes tick dir amount))))




; Reading Data ----------------------------------------------------------

(define target-path "input.txt")


(define-values (final-tick part1 part2)
  (for/fold ([tick 50]
             [zero-lands 0]
             [zero-touches 0])
            ([line (file->lines target-path)])
    (match-let ([(list dir amount) (string->instruction line)])
      (let-values ([(tick* dl dt) (turn tick dir amount)])
        (values
         tick*
         (+ zero-lands dl)
         (+ zero-touches dt))))))

; Part 1
part1

; Part 2
part2